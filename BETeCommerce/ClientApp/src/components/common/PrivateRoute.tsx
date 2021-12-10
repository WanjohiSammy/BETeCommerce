import { Navigate } from "react-router-dom";
import { useAuth } from "../customHooks/useAuth";

interface IPrivateRoute {
  children: any;
}

const PrivateRoute: React.FC<IPrivateRoute> = ({ children }) => {
  const auth = useAuth();

  return auth.isAuthenticated ? children : <Navigate to='/login' />;
};

export default PrivateRoute;
