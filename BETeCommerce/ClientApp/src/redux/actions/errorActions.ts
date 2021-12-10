import isEmpty from "../../validations/is-empty";
import { CLEAR_ERRORS, GET_ERRORS } from "../store/types";

// Set Errors
export const setErrors = (error: any) => {
  let payload = error;
  if (!isEmpty(error?.response?.data?.message)) {
    payload = error.response.data.message;
  }

  if (!isEmpty(error?.response?.data?.errors)) {
    payload = Object.values(error.response.data.errors).join("\n") as string;
  }

  if (isEmpty(error?.response) && !isEmpty(error?.message)) {
    payload = `${error.message}. Try again`;
  }

  const str = JSON.stringify(error);
  payload = str.includes("401")
    ? "Request failed. User not Authorized"
    : str.includes("500")
    ? "Request failed. Try again"
    : payload;

  return {
    type: GET_ERRORS,
    payload,
  };
};

// Clear Errors
export const clearErrors = () => {
  return {
    type: CLEAR_ERRORS,
  };
};
