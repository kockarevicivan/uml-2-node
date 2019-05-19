/**
 * @file Defines all user related route patterns.
 */
import express from 'express';

import Controller from '../controllers/Controller';
import { isAuthenticated } from '../middleware/authenticationMiddleware';

const router = express.Router({});

// Create
router.post('/', isAuthenticated, Controller.create);

// Read
router.get('/', isAuthenticated, Controller.getAll);
router.get('/:id', isAuthenticated, Controller.get);

// Update
router.put('/:id', isAuthenticated, Controller.update);

// Delete
router.delete('/:id', isAuthenticated, Controller.delete);

export default router;
