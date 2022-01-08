/* eslint-disable jsx-a11y/img-redundant-alt */

import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { BASE_URL_API } from "../../constants/constants";
import { addToCart, getCartItems } from "../../redux/actions/cartsActions";
import { formatNumber } from "../../utils";

interface ICard {
  product: any;
}

const Card: React.FC<ICard> = ({ product }) => {
  const url = BASE_URL_API + product.imageUrl;

  const dispatch = useDispatch();

  const storeCartItems = useSelector(
    (state: any) => state.cartsReducer.products
  ); 
  
  const [cartItems, setCartItems] = useState(storeCartItems);

  useEffect(() => {
    dispatch(getCartItems())
  },[dispatch]);

  useEffect(() => {
    setCartItems(storeCartItems)
  }, [storeCartItems]);

  const onAddToCart = () => {
    dispatch(addToCart(product));
  };

  const itemInCart = cartItems ? cartItems.filter((c: any) => c.id === product.id).length > 0 : false;

  return (
    <div className="card" style={{ width: "18rem" }}>
      <img
        className="card-img-top"
        src={url}
        alt="Card image cap"
        height="200px"
      />
      <div className="card-body">
        <p className="card-text lead">Name: {product.name}</p>
        <h4 className="card-text">Ksh: {formatNumber(product.price)}</h4>
        <button className={`btn btn-${itemInCart ? "danger" : "primary"}`} onClick={onAddToCart} disabled={itemInCart}>
          {itemInCart ? "In Cart" : "Add to Cart"}
        </button>
      </div>
    </div>
  );
};

export default Card;
