/**
 * @file Defines all entity related business logic.
 */
import bcrypt from 'bcryptjs';
import jwt from 'jsonwebtoken';
import { Document } from 'mongoose';

import config from '../config';
import  from '../models/';

class Service {
    /**
     * Returns an array containing all entities inside the db collection.
     */
    public getAll(): Promise<Document[]> {
        return new Promise<Document[]>((resolve, reject) => {
            .find()
                .then((data) => resolve(data))
                .catch((error) => reject('Couldn\'t find entities.'));
        });
    }

    /**
     * Returns a document containing data of the requested entity.
     * @param id Id of the wanted entity.
     */
    public get(id: string): Promise<Document> {
        return new Promise<Document>((resolve, reject) => {
            .findOne({ _id: id })
                .then((data) => resolve(data))
                .catch((error) => reject('Couldn\'t find entity.'));
        });
    }

    /**
     * Returns a document containing data of the requested entity.
     * @param email Email of the wanted entity.
     */
    public getByEmail(email: string): Promise<Document> {
        return new Promise<Document>((resolve, reject) => {
            .findOne({ email })
                .then((data) => resolve(data))
                .catch((error) => reject('Couldn\'t find entity.'));
        });
    }

    /**
     * Returns a document containing data of the requested entity.
     * @param refreshToken Refresh token of the wanted entity.
     */
    public getByRefreshToken(refreshToken: string): Promise<Document> {
        return new Promise<Document>((resolve, reject) => {
            .findOne({ refreshToken })
                .then((data) => resolve(data))
                .catch((error) => reject('Couldn\'t find entity.'));
        });
    }

    /**
     * Inserts a entity into database.
     * @param entity  that needs to be inserted.
     */
    public create(entity: any): Promise<Document> {
        return new Promise<Document>((resolve, reject) => {
            if (!entity.email) return reject('E-mail is mandatory.');
            if (!entity.fullName) return reject('Full name is mandatory.');
            if (!entity.password) return reject('Password is mandatory.');
            if (!entity.role) return reject('Role is mandatory.');

            bcrypt.genSalt(config.saltRounds, (saltError, salt) => {
                if (saltError) return reject('Salt error.');

                bcrypt.hash(entity.password, salt, (hashError, hash) => {
                    if (hashError) return reject('Hash error.');

                    const refreshToken =
                        jwt.sign({ data: { email: entity.email, fullName: entity.fullName } }, config.secret);

                    entity.refreshToken = refreshToken;
                    entity.password = hash;
                    entity.salt = salt;

                    .create(entity)
                        .then((data) => resolve(data))
                        .catch((error) => reject(' creation failed.'));
                });
            });
        });
    }

    /**
     * Updates all the data for the provided entity.
     * @param entity  object with new data applied.
     */
    public update(entity: any): Promise<any> {
        return new Promise<any>((resolve, reject) => {
            if (entity.password) {
                bcrypt.genSalt(config.saltRounds, (saltError, salt) => {
                    if (saltError) return reject('Salt error.');

                    bcrypt.hash(entity.password, salt, (hashError, hash) => {
                        if (hashError) return reject('Hash error.');

                        entity.password = hash;
                        entity.salt = salt;

                        .updateOne({ _id: entity.id }, { $set: entity }, { multi: true })
                            .then((data) => resolve(entity))
                            .catch((error) => reject(' update failed.'));
                    });
                });
            } else {
                .updateOne({ _id: entity.id }, { $set: entity }, { multi: true })
                    .then((data) => resolve(entity))
                    .catch((error) => reject(' update failed.'));
            }
        });
    }

    /**
     * Removes a specific entity from the db collection.
     * @param id Id of the entity that needs to be removed.
     */
    public delete(id: string): Promise<void> {
        return new Promise<void>((resolve, reject) => {
            .findByIdAndRemove(id)
                .then((data) => resolve())
                .catch((error) => reject(' removal failed.'));
        });
    }
}

export default new Service();
