import { useCart } from "../../components/customHooks/useCart";
import { CART_ITEMS } from "../../constants/constants";
import axiosApi from "../../helpers/axios";
import { ADD_TO_CART, CHECKOUT_LOADING, CHECKOUT_LOADING_CLEAR, DELETE_CART_ITEMS, GET_CART_ITEMS } from "../store/types";
import { clearErrors, setErrors } from "./errorActions";

export const setAddCart = (payload: number) => {
  return {
    type: ADD_TO_CART,
    payload,
  };
};

export const getCart = (payload: any) => {

  return {
    type: GET_CART_ITEMS,
    payload,
  };
};

export const clearLoading = (payload: any) => {

  return {
    type: CHECKOUT_LOADING_CLEAR,
    payload,
  };
};

export const setCheckoutLoading = () => {
  return {
    type: CHECKOUT_LOADING,
  };
};

export const addToCart = (product: any) => (dispatch: any) => {
  const cartItems = useCart();
  const currentItem = {
    id: product.id,
    categoryId: product.categoryId,
    name: product.name,
    imageUrl: product.imageUrl,
    quantity: product.quantity,
    quantityChosen: 1,
    price: product.price,
    count: 1,
  };

  let json = cartItems;
  if (json) {
    const { products } = json;

    json.count += 1;
    let index = -1;
    for (let i = 0; i < products.length; i++) {
      if (products[i].id === currentItem.id) {
        products[i].count += 1;
        index = i;
        break;
      }
    }

    if (index === -1) {
      products.push(currentItem);
    }

    json.products = products;
  } else {
    json = {
      products: [currentItem],
      count: 1,
    };
  }

  localStorage.setItem(CART_ITEMS, JSON.stringify(json));
  dispatch(setAddCart(json));
};

export const getCartItems = () => (dispatch: any) => {
  dispatch(getCart(useCart()));
};

export const removeCartItem = (productDta: any) => (dispatch: any) => {
  const cartItems = useCart();
  let json = cartItems;
  if (json) {
    const { products } = json;
    const newPoroducts = [];
    let count = 0;
    for (let i = 0; i < products.length; i++) {
      if (
        products[i].categoryId === productDta.categoryId ||
        products[i].id === productDta.productId
      ) {
        count++;
      } else {
        newPoroducts.push(products[i]);
      }
    }

    json.products = newPoroducts;
    json.count -= count;
  }

  localStorage.setItem(CART_ITEMS, JSON.stringify(json));
  dispatch(setAddCart(json));
};

export const deleteCartItems = () => (dispatch: any) => {
    localStorage.removeItem(CART_ITEMS);
    dispatch({
      type: DELETE_CART_ITEMS,
    });
    dispatch(clearLoading(""));
};

// Checkout
export const checkoutCartItems = (OrderData: object) => async (dispatch: any) => {
  try {
    dispatch(setCheckoutLoading())
    dispatch(clearErrors());
    const result = await axiosApi.postData("/Orders/Checkout", OrderData);
    
    dispatch(deleteCartItems());
  } catch (error: any) {
    dispatch(clearLoading(""));
    dispatch(setErrors(error));
  }
};
