import axiosApi from "../../helpers/axios";
import { ADD_CATEGORY, CATEGORY_LOADING, DELETE_CATEGORY, GET_CATEGORIES, UPDATE_CATEGORY } from "../store/types";
import { removeCartItem } from "./cartsActions";
import { clearErrors, setErrors } from "./errorActions";
import { setProducts } from "./productActions";

export const setCategories = (data: any) => {
  return {
    type: GET_CATEGORIES,
    payload: data,
  };
};

export const setCategory = (data: any) => {
  return {
    type: ADD_CATEGORY,
    payload: data,
  };
};

export const setCatLoading = () => {
  return {
    type: CATEGORY_LOADING,
  };
};

// Get Categories
export const getCategories =
  (pageNumber: number, pageSize: number) => async (dispatch: any) => {
    dispatch(setCatLoading());
    try {
      dispatch(clearErrors());
      const suffixUrl = `/Categories?pageNumber=${pageNumber}&pageSize=${pageSize}`;
      const result = await axiosApi.getData(suffixUrl);

      dispatch(setCategories(result.data));
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  };

export const getAllCategories = () => async (dispatch: any) => {
  dispatch(setCatLoading());
  try {
    dispatch(clearErrors());
    const suffixUrl = "/Categories/all";
    const result = await axiosApi.getData(suffixUrl);

    dispatch(setCategories(result.data));
  } catch (error: any) {
    dispatch(setErrors(error));
  }
};

// Get Category
export const getCategory = (catId: string) => async (dispatch: any) => {
  dispatch(setCatLoading());
  try {
    dispatch(clearErrors());
    const suffixUrl = `/Categories/${catId}`;
    const result = await axiosApi.getData(suffixUrl);

    dispatch(setCategory(result.data));
  } catch (error: any) {
    dispatch(setErrors(error));
  }
};

// Get GetProductsByCategories
export const getProductsByCategories =
  (id: string, pageNumber: number, pageSize: number) =>
  async (dispatch: any) => {
    dispatch(setCatLoading());
    try {
      dispatch(clearErrors());
      const suffixUrl = `/Categories/Products/${id}?pageNumber=${pageNumber}&pageSize=${pageSize}`;
      const result = await axiosApi.getData(suffixUrl);

      dispatch(setProducts(result.data?.categories[0]));
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  };

// Add Category
export const addCategory = (catData: object) => async (dispatch: any) => {
  dispatch(setCatLoading());
  try {
    dispatch(clearErrors());
    const result = await axiosApi.postData("/Categories", catData);

    dispatch(setCategory(result.data));
  } catch (error: any) {
    dispatch(setErrors(error));
  }
};

// Update Category
export const updateCategory = (catData: object) => async (dispatch: any) => {
  dispatch(setCatLoading());
  try {
    dispatch(clearErrors());
    const result = await axiosApi.putData("/Categories", catData);
    
    dispatch({
      type: UPDATE_CATEGORY,
      payload: result.data
    });
  } catch (error: any) {
    dispatch(setErrors(error));
  }
};

// Delete Category
export const deleteCategory = (catData: any) => async (dispatch: any) => {
  if (window.confirm("Are you sure? This can NOT be undone!")) {
    dispatch(setCatLoading());
    try {
      dispatch(clearErrors());
      const result = await axiosApi.deleteData("/Categories", catData);

      dispatch({
        type: DELETE_CATEGORY,
        payload: result.data
      });
      dispatch(removeCartItem({categoryId: catData.id}))
    } catch (error: any) {
      dispatch(setErrors(error));
    }
  }
};
