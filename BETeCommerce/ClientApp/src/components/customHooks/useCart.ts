import { CART_ITEMS } from "../../constants/constants";

export const useCart = () => {
    const cartItems = localStorage.getItem(CART_ITEMS) as any;

    if(!cartItems) return null;

    return JSON.parse(cartItems);
}