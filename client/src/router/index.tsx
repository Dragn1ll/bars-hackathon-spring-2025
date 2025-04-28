import AdminPage from "../pages/AdminPage";
import TableUsers from "../components/Users/TableUsers";
import PageNotFound from "../pages/PageNotFound";
import LoginForm from "../components/LoginForm";
import RegisterForm from "../components/RegisterForm";
import AdminForm from "../components/AdminForm";
import {Route} from "../types/types"
// import TableCourses from "../components/CourseStructure";


export const routes: Route[] = [
    { path: '/admin', element: <AdminPage />},
    { path: '/admin/users', element: <TableUsers /> },
    // { path: '/admin/courses', element: <TableCourses /> },
    { path: '/login', element: <LoginForm title={"Авторизируйтесь"} /> },
    { path: '/register', element: <RegisterForm title={"Регистрация"} /> },
    { path: '/admin-register', element: <AdminForm title={"Вход в админ-панель"} /> },
    { path: '*', element: <PageNotFound /> },
];