/**
 * @file Defines all user related route patterns.
 */
import express from 'express';

import {{EntityNamePascalCase}}Controller from '../controllers/{{EntityNamePascalCase}}Controller';
import { isAuthenticated } from '../middleware/authenticationMiddleware';

const router = express.Router({});

// Create
router.post('/', isAuthenticated, {{EntityNamePascalCase}}Controller.create);

// Read
router.get('/', isAuthenticated, {{EntityNamePascalCase}}Controller.getAll);
router.get('/:id', isAuthenticated, {{EntityNamePascalCase}}Controller.get);

// Update
router.put('/:id', isAuthenticated, {{EntityNamePascalCase}}Controller.update);

// Delete
router.delete('/:id', isAuthenticated, {{EntityNamePascalCase}}Controller.delete);

export default router;
