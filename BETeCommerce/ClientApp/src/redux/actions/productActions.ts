import axiosApi from "../../helpers/axios";
import {
  ADD_PRODUCT,
  DELETE_PRODUCT,
  GET_PRODUCTS,
  PRODUCT_LOADING,
  REMOVE_FROM_CART,
  UPDATE_PRODUCT,
} from "../store/types";
import { removeCartItem } from "./cartsActions";
import { clearErrors, setErrors } from "./errorActions";

export const setProducts = (data: any) => {
  return {
    type: GET_PRODUCTS,
    payload: data,
  };
};

export const setProduct = (data: any) => {
  return {
    type: ADD_PRODUCT,
    payload: data,
  };
};

export const setProdLoading = () => {
  return {
    type: PRODUCT_LOADING,
  };
};

// Get Products
export const getProducts =
  (pageNumber: number, pageSize: number) => async (dispatch: any) => {
    dispatch(setProdLoading());
    try {
      dispatch(clearErrors());
      const suffixUrl = `/Products?pageNumber=${pageNumber}&pageSize=${pageSize}`;
      const result = await axiosApi.getData(suffixUrl);

      dispatch(setProducts(result.data));
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  };

// Get Product
export const getProduct = (id: string) => async (dispatch: any) => {
  dispatch(setProdLoading());
  try {
    dispatch(clearErrors());
    const suffixUrl = `/Products/${id}`;
    const result = await axiosApi.getData(suffixUrl);

    dispatch(setProduct(result.data));
  } catch (error: any) {
    dispatch(setErrors(error));
  }
};

// Add Product
export const addProduct = (productData: object) => async (dispatch: any) => {
  dispatch(setProdLoading());
  try {
    dispatch(clearErrors());
    const result = await axiosApi.postData("/Products", productData);

    dispatch(setProduct(result.data));
  } catch (error: any) {
    dispatch(setErrors(error));
  }
};

// Update Product
export const updateProductDetails =
  (productData: object) => async (dispatch: any) => {
    console.log(productData);

    dispatch(setProdLoading());
    try {
      dispatch(clearErrors());
      const result = await axiosApi.putData("/Products", productData);

      dispatch({
        type: UPDATE_PRODUCT,
        payload: result.data,
      });
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  };

// Update Product Image
export const updateProductImage =
  (imageData: object) => async (dispatch: any) => {
    dispatch(setProdLoading());
    try {
      dispatch(clearErrors());
      const result = await axiosApi.putData("/Products/UpdateImage", imageData);

      dispatch(setProduct(result.data));
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  };

// Delete Product
export const deleteProduct = (productData: any) => async (dispatch: any) => {
  if (window.confirm("Are you sure? This can NOT be undone!")) {
    dispatch(setProdLoading());
    try {
      dispatch(clearErrors());
      const result = await axiosApi.deleteData("/Products", productData);

      dispatch({
        type: DELETE_PRODUCT,
        payload: result.data,
      });
      dispatch(removeCartItem({productId: productData.id}))
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  }
};
