import jwtDecode from "jwt-decode";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";
import { JWT_TOKEN_NAME } from "./../constants/constants";
import setAuthToken from "./../utils/setAuthToken";
import {
  logoutUser,
  setCurrentUser
} from "./../redux/actions/authActions";

const AuthProvider = (props: any) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const auth: any = useSelector((state: any) => state.authReducer.isAuthenticated);

  useEffect(() => {
    const token = localStorage.getItem(JWT_TOKEN_NAME);
    // dispatch(
    //   loginUser({
    //     Email: "TEST1@grmadil.com",
    //     Password: "wQ23443!sf",
    //   })
    // );
    if (token) {
      // Set auth token header auth
      setAuthToken(token);

      // Decode token and get user info and exp
      const decoded = jwtDecode(token) as any;

      // Set user and isAuthentcated
      dispatch(setCurrentUser(decoded));

      // Check for expired token
      const currentTime = Date.now() / 1000;
      if (decoded.exp < currentTime) {
        // Logout user
        dispatch(logoutUser());

        // Clear current Profile
        // store.dispatch(clearCurrentProfile());

        // Regdirect to login
        navigate("/login");
      }
    }
  }, [auth]);
  
  return props.children;
};

export default AuthProvider;
