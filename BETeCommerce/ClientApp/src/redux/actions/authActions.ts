import jwtDecode from "jwt-decode";
import { JWT_TOKEN_NAME } from "../../constants/constants";
import setAuthToken from "../../utils/setAuthToken";
import { CLEAR_USER_LOADING, CURRENT_USER_LOADING, SET_CURRENT_USER } from "../store/types";
import axiosApi from "./../../helpers/axios/index";
import { clearErrors, setErrors } from "./errorActions";

// Set logged in user
export const setCurrentUser = (decoded: any) => {
  return {
    type: SET_CURRENT_USER,
    payload: decoded,
  };
};

export const setUserLoading = () => {
  return {
    type: CURRENT_USER_LOADING
  };
};

export const clearUserLoading = () => {
  return {
    type: CLEAR_USER_LOADING
  };
};

// Register User
export const registerUser = (userData: object) => async (dispatch: any) => {
  dispatch(setUserLoading());
  try {
    dispatch(clearErrors());
    const result = await axiosApi.postData("/Users/RegisterUser", userData);

    dispatch(clearUserLoading());
    dispatch(setCurrentUser(result.data));
    
    // Redirect to Login
    window.location.href = "/login";
  } catch (error: any) {
    dispatch(clearUserLoading());
    dispatch(setErrors(error));
  }
};

// Login - Get User Token
export const loginUser = (userData: object) => async (dispatch: any) => {
  dispatch(setUserLoading());
  try {
    dispatch(clearErrors());
    const result = await axiosApi.postData("/Users/Login", userData);
    const { accessToken } = result.data?.userDetails;
    // Set token to localStorage
    window.localStorage.setItem(JWT_TOKEN_NAME, accessToken);

    // Set token to Auth header
    setAuthToken(accessToken);

    // Decode token to get user data
    const decoded = jwtDecode(accessToken);

    
    dispatch(clearUserLoading());
    // Set current user
    dispatch(setCurrentUser(decoded));
  } catch (error: any) {
    
    dispatch(clearUserLoading());
    dispatch(setErrors(error));
  }
};

// Log user out
export const logoutUser = () => (dispatch: any) => {
  //  Remove token from localStorage
  localStorage.removeItem(JWT_TOKEN_NAME);

  // Remove auth header for future requests
  setAuthToken(false);

  // Set current user to { } which will set isAuthenticated to false
  dispatch(setCurrentUser({}));

  // TODO: Clear current Profile

  // Redirect to login
  window.location.href = "/login";
};
