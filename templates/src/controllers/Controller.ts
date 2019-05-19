/**
 * @file Dispatcher for entity related requests.
 */
import express from 'express';
import {{EntityNamePascalCase}}Service from '../services/{{EntityNamePascalCase}}Service';

class {{EntityNamePascalCase}}Controller {
    public getAll(req: express.Request, res: express.Response) {
        {{EntityNamePascalCase}}Service.getAll()
            .then((entities: []) => res.send({
                data: entities,
                success: true,
            }))
            .catch((errorMessage: string) => res.send({
                message: errorMessage,
                success: false,
            }));
    }

    public get(req: express.Request, res: express.Response) {
        {{EntityNamePascalCase}}Service.get(req.params.id)
            .then((entity: any) => res.send({
                data: entity,
                success: true,
            }))
            .catch((errorMessage: string) => res.send({
                message: errorMessage,
                success: false,
            }));
    }

    public create(req: express.Request, res: express.Response) {
        {{EntityNamePascalCase}}Service.create({
            {{FieldsAsJSON}}
            //email: req.body.email,
            //fullName: req.body.fullName,
            //password: req.body.password,
            //refreshToken: req.body.refreshToken,
            //role: req.body.role,
        })
            .then((entity: any) => res.send({
                data: entity,
                success: true,
            }))
            .catch((errorMessage: string) => res.send({
                message: errorMessage,
                success: false,
            }));
    }

    public update(req: express.Request, res: express.Response) {
        {{EntityNamePascalCase}}Service.update({
			id: req.params.id,
			{{FieldsAsJSON}}
            //email: req.body.email,
            //fullName: req.body.fullName,
            //password: req.body.password,
            //refreshToken: req.body.refreshToken,
            //role: req.body.role,
        })
            .then((entity: any) => res.send({
                data: entity,
                success: true,
            }))
            .catch((errorMessage: string) => res.send({
                message: errorMessage,
                success: false,
            }));
    }

    public delete(req: express.Request, res: express.Response) {
        {{EntityNamePascalCase}}Service.delete(req.params.id)
            .then(() => res.send({
                success: true,
            }))
            .catch((errorMessage: string) => res.send({
                message: errorMessage,
                success: false,
            }));
    }
}

export default new {{EntityNamePascalCase}}Controller();
