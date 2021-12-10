import jwtDecode from "jwt-decode";
import { JWT_TOKEN_NAME } from "./../../constants/constants";

export const useAuth = () => {
  const token = localStorage.getItem(JWT_TOKEN_NAME);
  if (token) {
    const decoded = jwtDecode(token) as any;

    // Check for expired token
    const currentTime = Date.now() / 1000;
    if (decoded.exp < currentTime) {
      return { user: {}, isAuthenticated: false };
    }
    return { user: decoded, isAuthenticated: true };
  }
  
  return { user: {}, isAuthenticated: false };
};
