/**
 * @file Defines all authentication related route patterns.
 */
import express from 'express';

import AuthenticationController from '../controllers/AuthenticationController';

const router = express.Router({});

// Generate
router.post('/', AuthenticationController.generateToken);

// Refresh
router.post('/refresh', AuthenticationController.refreshToken);

export default router;
