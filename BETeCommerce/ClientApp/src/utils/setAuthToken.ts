import axios from "axios";
import { JWT_BEARER } from "../constants/constants";

const setAuthToken = (token: any) => {
  if (token) {
    // Apply to every request
    axios.defaults.headers.common["Authorization"] = JWT_BEARER + token;
  } else {
    // Delete auth header
    delete axios.defaults.headers.common["Authorization"];
  }
};

export default setAuthToken;
