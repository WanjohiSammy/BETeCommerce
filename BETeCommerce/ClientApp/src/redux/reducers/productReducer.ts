import {
  ADD_PRODUCT,
  DELETE_PRODUCT,
  GET_PRODUCT,
  GET_PRODUCTS,
  PRODUCT_LOADING,
  UPDATE_PRODUCT,
} from "../store/types";

const initialState = {
  products: [],
  product: {},
  count: 0,
  loading: false,
};

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = initialState, action: any) {
  switch (action.type) {
    case PRODUCT_LOADING:
      return {
        ...state,
        loading: true,
      };

    case GET_PRODUCTS:
      return {
        ...state,
        products: action.payload.products,
        count: action.payload.count,
        loading: false,
      };

    case GET_PRODUCT:
      return {
        ...state,
        product: action.payload.product,
        loading: false,
      };

    case ADD_PRODUCT:
      return {
        ...state,
        product: action.payload.product,
        products: [action.payload.product, ...state.products],
        loading: false,
      };

    case UPDATE_PRODUCT:
      return {
        ...state,
        product: action.payload.product,
        products:
          state.products.map(
            (prod: any) => prod.id === action.payload.product.id ? action.payload.product : prod
          ),
        loading: false,
      };

    case DELETE_PRODUCT:
      return {
        ...state,
        products: state.products.filter(
          (prod: any) => prod.id !== action.payload.product.id
        ),
        loading: false,
      };

    default:
      return state;
  }
}
