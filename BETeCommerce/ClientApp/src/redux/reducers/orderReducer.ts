import {
  ADD_ORDER,
  DELETE_ORDER,
  GET_ORDER,
  GET_ORDERS,
  ORDER_LOADING,
  UPDATE_ORDER,
} from "../store/types";

const initialState = {
  orders: [],
  order: {},
  count: 0,
  loading: false,
};

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = initialState, action: any) {
  switch (action.type) {
    case ORDER_LOADING:
      return {
        ...state,
        loading: true,
      };

    case GET_ORDERS:
      return {
        ...state,
        orders: action.payload.orders,
        count: action.payload.count,
        loading: false,
      };

    case GET_ORDER:
      return {
        ...state,
        order: action.payload.order,
        loading: false,
      };

    case ADD_ORDER:
      return {
        ...state,
        order: action.payload.order,
        orders: [action.payload.order, ...state.orders],
        loading: false,
      };

    case UPDATE_ORDER:
      return {
        ...state,
        order: action.payload.order,
        orders:
          state.orders.map(
            (ord: any) => ord.id === action.payload.order.id ?  action.payload.order : ord
          ),
        loading: false,
      };

    case DELETE_ORDER:
      return {
        ...state,
        orders: state.orders.filter(
          (ord: any) => ord.id !== action.payload.order.id
        ),
        loading: false,
      };

    default:
      return state;
  }
}
