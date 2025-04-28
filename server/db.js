import { Pool } from 'pg';


const pool = new Pool({
    host: 'localhost',
    user: 'postgres',
    post: 5432,
    password: '1234',
    database: 'testDB',
});

export default pool;