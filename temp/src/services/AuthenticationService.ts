/**
 * @file Defines all authentication related business logic.
 */
import bcrypt from 'bcryptjs';
import jwt from 'jsonwebtoken';

import config from '../config';
import Service from './Service';

class AuthenticationService {
    /**
     * Generates a JWT token, based on the provided login data.
     * @param email Email of the identity that requests the token.
     * @param password Password of the identity that requests the token.
     */
    public generateToken(email: string, password: string): Promise<string> {
        return new Promise<string>((resolve, reject) => {
            if (!email) return reject('Email is required.');
            if (!password) return reject('Password is required.');

            Service.getByEmail(email).then((identity: any) => {
                if (!identity) return reject(' not found.');

                bcrypt.hash(password, identity.salt, (hashError, hash) => {
                    if (hashError) return reject('Hash error.');
                    if (hash !== identity.password) return reject('Passwords don\'t match.');

                    const accessToken = jwt.sign({
                        data: {
                            email: identity.email,
                            expiresAt: new Date().getTime() + config.tokenExpirySeconds,
                            fullName: identity.fullName,
                        },
                    }, config.secret, { expiresIn: config.tokenExpirySeconds });

                    resolve(accessToken);
                });
            }).catch((error: string) => reject(error));
        });
    }

    /**
     * Refreshes the token by provided refresh token.
     * @param refreshToken Refresh token for the identity that needs to refresh the token.
     */
    public refreshToken(refreshToken: string): Promise<string> {
        return new Promise<string>((resolve, reject) => {
            if (!refreshToken) return reject('Email is required.');

            Service.getByRefreshToken(refreshToken).then((identity: any) => {
                if (!identity) return reject(' not found.');

                const accessToken = jwt.sign({
                    data: {
                        email: identity.email,
                        expiresAt: new Date().getTime() + config.tokenExpirySeconds,
                        fullName: identity.fullName,
                    },
                }, config.secret, { expiresIn: config.tokenExpirySeconds });

                resolve(accessToken);
            }).catch((error: string) => reject(error));
        });
    }
}

export default new AuthenticationService();
