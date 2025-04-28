import { Navigate, Outlet } from 'react-router-dom';
import {IProtectedRouteProps} from "../types/types"


export const ProtectedRoute = ({
                                   isAllowed,
                                   redirectPath = '/login',
                                   children,
                               }: IProtectedRouteProps) => {
    if (!isAllowed) {
        return <Navigate to={redirectPath} replace />;
    }

    return children ? children : <Outlet />;
};