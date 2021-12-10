import {
  ADD_TO_CART,
  CHECKOUT_LOADING,
  CHECKOUT_LOADING_CLEAR,
  DELETE_CART_ITEMS,
  GET_CART_ITEMS,
  REMOVE_FROM_CART,
} from "../store/types";

const initialState = {
  count: 0,
  products: [],
  loading: false,
};

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = initialState, action: any) {
  switch (action.type) {
    case CHECKOUT_LOADING_CLEAR:
      return {
        loading: false,
      };

    case CHECKOUT_LOADING:
      return {
        loading: true,
      };

    case ADD_TO_CART:
      return {
        count: action.payload?.count,
        products: action.payload?.products,
        loading: false,
      };

    case GET_CART_ITEMS:
      return {
        count: action.payload?.count,
        products: action.payload?.products,
        loading: false,
      };

    case REMOVE_FROM_CART:
      return {
        count: action.payload?.count,
        products: action.payload?.products,
        loading: false,
      };

    case DELETE_CART_ITEMS:
      return {
        count: 0,
        products: [],
        loading: false,
      };

    default:
      return state;
  }
}
