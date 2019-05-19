/**
 * @file Dispatcher for entity related requests.
 */
import express from 'express';
import Service from '../services/Service';

class Controller {
    public getAll(req: express.Request, res: express.Response) {
        Service.getAll()
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
        Service.get(req.params.id)
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
        Service.create({
            	ldProjectDocument: string,
	ldProject: string,
	IdDocument: string,

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
        Service.update({
			id: req.params.id,
				ldProjectDocument: string,
	ldProject: string,
	IdDocument: string,

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
        Service.delete(req.params.id)
            .then(() => res.send({
                success: true,
            }))
            .catch((errorMessage: string) => res.send({
                message: errorMessage,
                success: false,
            }));
    }
}

export default new Controller();
