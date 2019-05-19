/**
 * @file Defines all constants required for application configuration.
 */
const config = {
    databaseUrl: 'mongodb://localhost/{{ProjectNameDashCase}}',
    port: 3000,
    saltRounds: 10,
    secret: 'qwertyuiopasdfghjklzxcvbnm1234567890',
    tokenExpirySeconds: 14400,
};

export default config;
