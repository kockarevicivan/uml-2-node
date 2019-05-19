using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Tesseract;
using Uml2Node.Core.Interfaces;
using Uml2Node.Core.Model;
using Uml2Node.Parser.Factories;
using Uml2Node.Parser.Helpers;

namespace Uml2Node.Parser.Services
{
    public class ParserService : IParserService
    {
        /// <inheritdoc />
        public List<Entity> Parse(string imagePath)
        {
            FileHelper.ClearTempDirectory();

            this.Slice(imagePath);

            string[] filePaths = Directory.GetFiles("temp");

            if (filePaths.Length == 0)
                throw new Exception("No processed images found!");

            List<string> entityStrings = this.ProcessTables(filePaths.ToList());

            List<Entity> entities = new List<Entity>();

            foreach (string entityString in entityStrings)
                entities.Add(ModelFactory.CreateEntity(entityString));

            return entities;
        }

        /// <summary>
        /// Detects squares inside the provided image and makes image slices for each square.
        /// </summary>
        /// <param name="imagePath">Path to the diagram image.</param>
        /// <returns>Bitmap of the diagram image with marked table squares.</returns>
        private Bitmap Slice(string imagePath)
        {
            Bitmap image = PreProcess((Bitmap)Bitmap.FromFile(imagePath));
            Bitmap fakeImage = new Bitmap(image.Width, image.Height);

            BlobCounter blobCounter = new BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 80;
            blobCounter.MinWidth = 80;

            blobCounter.ProcessImage(image);

            Blob[] blobs = blobCounter.GetObjectsInformation();

            var maxArea = blobs.Max(b => b.Area);
            blobs = blobs.Where(b => b.Area != maxArea).ToArray();

            // check for rectangles
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            Graphics g = Graphics.FromImage(fakeImage);
            g.DrawImage(image, 0, 0, image.Width, image.Height);

            foreach (var blob in blobs)
            {
                Bitmap cropped = Crop(image, blob.Rectangle.X, blob.Rectangle.Y, blob.Rectangle.Width, blob.Rectangle.Height);

                cropped.Save(@"temp/" + DateTime.Now.Ticks + ".png");

                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blob);
                List<IntPoint> cornerPoints;

                shapeChecker.IsQuadrilateral(edgePoints, out cornerPoints);

                List<System.Drawing.Point> Points = new List<System.Drawing.Point>();

                foreach (var point in cornerPoints)
                    Points.Add(new System.Drawing.Point(point.X, point.Y));

                g.DrawPolygon(new Pen(Color.Red, 5.0f), Points.ToArray());
            }

            return fakeImage;
        }

        /// <summary>
        /// Crops the bitmap of the diagram.
        /// </summary>
        /// <param name="image">Bitmap of the entire diagram image.</param>
        /// <param name="x">X coordinate for crop begin.</param>
        /// <param name="y">Y coordinate for crop begin.</param>
        /// <param name="width">Width of the crop.</param>
        /// <param name="height">Height of the crop.</param>
        /// <returns>Bitmap of the cropped image.</returns>
        private Bitmap Crop(Bitmap image, int x, int y, int width, int height)
        {
            return new Crop(new Rectangle(x, y, width, height)).Apply(image);
        }

        /// <summary>
        /// Applies the grayscale and treshold filter to the original bitmap.
        /// </summary>
        /// <param name="bmp">Original bitmap.</param>
        /// <returns>Bitmap with filters applied.</returns>
        private Bitmap PreProcess(Bitmap bmp)
        {
            bmp = new Grayscale(0.2125, 0.7154, 0.0721).Apply(bmp);
            new BradleyLocalThresholding().ApplyInPlace(bmp);

            return bmp;
        }

        /// <summary>
        /// Processes each table image separately.
        /// </summary>
        /// <param name="imagePaths">Image of the each table from the diagram.</param>
        /// <returns>List of text contents for each table.</returns>
        private List<string> ProcessTables(List<string> imagePaths)
        {
            List<string> entityStrings = new List<string>();

            using (var engine = new TesseractEngine(@"../../../tessdata", "eng", EngineMode.Default))
            {
                foreach (string imagePath in imagePaths)
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            entityStrings.Add(page.GetText());
                        }
                    }
                }
            }

            return entityStrings;
        }
    }
}
