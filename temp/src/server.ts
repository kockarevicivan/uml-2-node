/**
 * @file Startup file.
 */
import bluebird from 'bluebird';
import bodyParser from 'body-parser';
import express from 'express';
import mongoose from 'mongoose';

import config from './config';
import authenticationRoutes from './routes/authentication';

import userproiectroleRoutes from './routes/userproiectrole';
import mRoutes from './routes/m';
import timesheethourRoutes from './routes/timesheethour';
import companyRoutes from './routes/company';



// Initialize Express.
const app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

// Configure promise with Bluebird and connect to MongoDB.
mongoose.Promise = bluebird;
mongoose.connect(config.databaseUrl, { useNewUrlParser: true });

// Map routes.
app.use('/userproiectrole', userproiectroleRoutes);
app.use('/m', mRoutes);
app.use('/timesheethour', timesheethourRoutes);
app.use('/company', companyRoutes);


app.use('/authentication', authenticationRoutes);

// Start the app.
app.listen(3000, () => console.log(`App listening on http://localhost:${config.port}`));
