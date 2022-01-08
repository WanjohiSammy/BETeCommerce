import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { BASE_URL_API } from "../../constants/constants";
import {
  checkoutCartItems,
  deleteCartItems,
  getCartItems,
  removeCartItem,
} from "../../redux/actions/cartsActions";
import { formatNumber } from "../../utils";
import Spinner from "../common/Spinner";
import { useAuth } from "../customHooks/useAuth";
import { Alert } from "../errors/Alert";

const Checkout = () => {
  const dispatch = useDispatch();
  const auth = useAuth();

  const storeCartItems = useSelector(
    (state: any) => state.cartsReducer.products
  );

  const storeLoading = useSelector((state: any) => state.cartsReducer.loading);

  const [cartItems, setCartItems] = useState(storeCartItems);
  const [totalPrices, setTotalPrices] = useState(0);
  const [test, setTest] = useState(1);
  const [successCheckout, setSuccessCheckout] = useState(false);
  const [loading, setLoading] = useState(storeLoading);

  useEffect(() => {
    dispatch(getCartItems());
  }, [dispatch]);

  useEffect(() => {
    setCartItems(storeCartItems);
  }, [storeCartItems]);

  useEffect(() => {
    setLoading(storeLoading);
  }, [storeLoading]);

  const onDeleteFromCart = (e: any) => {
    dispatch(removeCartItem({ productId: e.target.id }));
  };

  const onClearCart = () => {
    dispatch(deleteCartItems());
  };

  const cartItemsTemp = cartItems;
  const modifyQuantity = (id: string, type: string) => {
    setTest(test + 1);
    for (let i = 0; i < cartItemsTemp.length; i++) {
      const item = cartItemsTemp[i];

      if (item.id === id) {
        const { quantityChosen } = item;
        const newObj = {
          ...item,
          quantityChosen:
            type === "+"
              ? quantityChosen + 1
              : type === "-"
              ? quantityChosen - 1
              : quantityChosen,
        };

        cartItemsTemp[i] = newObj;
      }
    }

    setCartItems(cartItemsTemp);
  };

  const checkoutHandle = (e: any) => {
    e.preventDefault();
    var checkoutObj = {
      buyerEmail: auth.user.email,
      cartItems: [] as Array<any>,
      totalPrice: totalPrices,
    };

    for (let item of cartItems) {
      checkoutObj.cartItems.push({
        productName: item.name,
        quantity: item.quantityChosen,
        price: item.price * item.quantityChosen,
      });
    }

    dispatch(checkoutCartItems(checkoutObj));
    setSuccessCheckout(true);
  };

  let totalPriceTmpt = 0;
  if (cartItems) {
    for (let i = 0; i < cartItems.length; i++) {
      const item = cartItems[i];
      totalPriceTmpt += item.quantityChosen * item.price;
    }
  }

  useEffect(() => {
    setTotalPrices(totalPriceTmpt);
  }, [totalPriceTmpt]);

  return (
    <div className="container">
      <div className="text-center">
        {successCheckout && !(cartItems && cartItems.length > 0) && (
          <Alert
            alertName="success"
            errors={"Checkout sent successfully. Check your Email."}
          />
        )}
      </div>
      <table className="table table-bordered table-responsive-md">
        <thead>
          <tr>
            <th scope="col">#</th>
            <th scope="col">Image</th>
            <th scope="col">Item</th>
            <th scope="col">Quantity</th>
            <th scope="col">Price</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          {cartItems &&
            cartItems.map((cart: any, index: number) => (
              <tr key={cart.id}>
                <th scope="row">{++index}</th>
                <td>
                  <img
                    src={BASE_URL_API + cart.imageUrl}
                    height="150px"
                    width="200px"
                    alt=""
                  />
                </td>
                <td>{cart.name}</td>
                <td>
                  <div className="input-group">
                    <span className="input-group-btn">
                      <button
                        className="quantity-left-minus btn btn-danger btn-number"
                        datatype="minus"
                        name={cart.id}
                        disabled={cart.quantityChosen === 1}
                        onClick={(e) => modifyQuantity(cart.id, "-")}
                      >
                        <span className="glyphicon glyphicon-minus">-</span>
                      </button>
                    </span>
                    <div className="m-2">{cart.quantityChosen}</div>
                    <span className="input-group-btn">
                      <button
                        className="quantity-right-plus btn btn-success btn-number"
                        datatype="plus"
                        name={cart.id}
                        disabled={cart.quantityChosen === cart.quantity}
                        onClick={(e) => modifyQuantity(cart.id, "+")}
                      >
                        <span className="glyphicon glyphicon-plus">+</span>
                      </button>
                    </span>
                  </div>
                </td>
                <td>{formatNumber(cart.price * cart.quantityChosen)}</td>
                <td>
                  <button
                    id={cart.id}
                    className="btn btn-sm btn-danger"
                    onClick={onDeleteFromCart}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))}

          <tr className="p-3 mb-2 bg-dark text-white">
            <th scope="row"></th>
            <td colSpan={3}>
              <h3>Total Price</h3>
            </td>
            <td>
              <h3>
                <strong>{formatNumber(totalPrices)}</strong>
              </h3>
            </td>
            <td></td>
          </tr>
        </tbody>
      </table>
      <div style={{ textAlign: "right" }}>
        <button
          className="btn btn-lg btn-danger mr-3"
          onClick={onClearCart}
          disabled={!cartItems || cartItems.length === 0}
        >
          CLear Cart
        </button>
        <button
          className="btn btn-lg btn-success"
          disabled={!cartItems || cartItems.length === 0 || loading}
          onClick={checkoutHandle}
        >
          Checkout
        </button>
        {loading && <Spinner />}
      </div>
    </div>
  );
};

export default Checkout;
