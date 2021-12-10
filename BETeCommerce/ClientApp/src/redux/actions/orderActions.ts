import axiosApi from "../../helpers/axios";
import {
  ADD_ORDER,
  DELETE_ORDER,
  GET_ORDERS,
  ORDER_LOADING,
  UPDATE_ORDER,
} from "../store/types";
import { clearErrors, setErrors } from "./errorActions";

export const setOrders = (data: any) => {
  return {
    type: GET_ORDERS,
    payload: data,
  };
};

export const setOrder = (data: any) => {
  return {
    type: ADD_ORDER,
    payload: data,
  };
};

export const setOrderLoading = () => {
  return {
    type: ORDER_LOADING,
  };
};

// Get Orders
export const getOrders =
  (pageNumber: number, pageSize: number) => async (dispatch: any) => {
    dispatch(setOrderLoading());
    try {
      dispatch(clearErrors());
      const suffixUrl = `/Orders?pageNumber=${pageNumber}&pageSize=${pageSize}`;
      const result = await axiosApi.getData(suffixUrl);

      dispatch(setOrders(result.data));
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  };

export const getBuyersOrders =
  (pageNumber: number, pageSize: number) => async (dispatch: any) => {
    dispatch(setOrderLoading());
    try {
      dispatch(clearErrors());
      const suffixUrl = `/Orders/Buyers?pageNumber=${pageNumber}&pageSize=${pageSize}`;
      const result = await axiosApi.getData(suffixUrl);

      dispatch(setOrders(result.data));
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  };

// Get Order
export const getOrder = (id: string) => async (dispatch: any) => {
  dispatch(setOrderLoading());
  try {
    dispatch(clearErrors());
    const suffixUrl = `/Orders/${id}`;
    const result = await axiosApi.getData(suffixUrl);

    dispatch(setOrder(result.data));
  } catch (error: any) {
    dispatch(setErrors(error));
  }
};

// Add Order
export const addOrder = (OrderData: object) => async (dispatch: any) => {
  dispatch(setOrderLoading());
  try {
    dispatch(clearErrors());
    const result = await axiosApi.postData("/Orders", OrderData);

    dispatch(setOrder(result.data));
  } catch (error: any) {
    dispatch(setErrors(error));
  }
};

// Update Order
export const updateOrderDetails =
  (OrderData: object) => async (dispatch: any) => {
    dispatch(setOrderLoading());
    try {
      dispatch(clearErrors());
      const result = await axiosApi.putData("/Orders", OrderData);

      dispatch({
        type: UPDATE_ORDER,
        payload: result.data,
      });
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  };

// Delete Order
export const deleteOrder = (OrderData: object) => async (dispatch: any) => {
  if (window.confirm("Are you sure? This can NOT be undone!")) {
    dispatch(setOrderLoading());
    try {
      dispatch(clearErrors());
      const result = await axiosApi.deleteData("/Orders", OrderData);

      dispatch({
        type: DELETE_ORDER,
        payload: result.data,
      });
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  }
};
