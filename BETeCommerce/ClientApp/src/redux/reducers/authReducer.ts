import {
  CLEAR_USER_LOADING,
  CURRENT_USER_LOADING,
  SET_CURRENT_USER,
} from "./../store/types";
import isEmpty from "./../../validations/is-empty";

const initialState = {
  isAuthenticated: false,
  user: {},
  loading: false,
};

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = initialState, action: any) {
  switch (action.type) {
    case CURRENT_USER_LOADING:
      return {
        loading: true,
      };

    case CLEAR_USER_LOADING:
      return {
        loading: false,
      };

    case SET_CURRENT_USER:
      return {
        ...state,
        isAuthenticated:
          !isEmpty(action.payload?.userDetails?.accessToken) ||
          !isEmpty(action.payload?.nameid),
        user: action.payload,
        loading: false,
      };

    default:
      return state;
  }
}
