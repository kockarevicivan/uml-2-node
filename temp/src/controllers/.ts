/**
 * @file Defines a schema for the user collection.
 */
import mongoose from 'mongoose';

import { transform } from '../utils/transform';

export default mongoose.model('', new mongoose.Schema({
		ldProjectDocument: string,
	ldProject: string,
	IdDocument: string,

    //email: String,
    //fullName: String,
    //password: String,
    //refreshToken: String,
    //role: String,
    //salt: String,
}, {
    timestamps: true,
    toJSON: { transform },
}));
