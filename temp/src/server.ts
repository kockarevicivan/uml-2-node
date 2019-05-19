/**
 * @file Startup file.
 */
import bluebird from 'bluebird';
import bodyParser from 'body-parser';
import express from 'express';
import mongoose from 'mongoose';

import config from './config';
import authenticationRoutes from './routes/authentication';

import timesheetcostcenterRoutes from './routes/timesheetcostcenter';
import mRoutes from './routes/m';
import proiectcostcenterRoutes from './routes/proiectcostcenter';
import proiectdocumentRoutes from './routes/proiectdocument';



// Initialize Express.
const app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

// Configure promise with Bluebird and connect to MongoDB.
mongoose.Promise = bluebird;
mongoose.connect(config.databaseUrl, { useNewUrlParser: true });

// Map routes.
app.use('/timesheetcostcenter', timesheetcostcenterRoutes);
app.use('/m', mRoutes);
app.use('/proiectcostcenter', proiectcostcenterRoutes);
app.use('/proiectdocument', proiectdocumentRoutes);


app.use('/authentication', authenticationRoutes);

// Start the app.
app.listen(3000, () => console.log(`App listening on http://localhost:${config.port}`));
