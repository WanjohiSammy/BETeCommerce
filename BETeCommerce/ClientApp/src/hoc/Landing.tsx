import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";
import Card from "../components/shopping/Card";
import { useAuth } from "../components/customHooks/useAuth";
import { getCategories } from "../redux/actions/categoryActions";
import { getProducts } from "../redux/actions/productActions";
import { BASE_URL_API } from "../constants/constants";

const Landing = () => {
  const dispatch = useDispatch();
  const auth = useAuth();
  const { isAuthenticated } = auth;

  const storeCategories = useSelector(
    (state: any) => state.categoryReducer.categories
  );

  const storeProducts = useSelector(
    (state: any) => state.productReducer.products
  );

  const [categories, setCategories] = useState(storeCategories);
  const [products, setProducts] = useState(storeProducts);

  useEffect(() => {
    dispatch(getCategories(1, 4));
    dispatch(getProducts(1, 5));
  }, [dispatch]);

  useEffect(() => {
    dispatch(getCategories(1, 4));
  }, [dispatch]);

  useEffect(() => {
    setCategories(storeCategories);
  }, [storeCategories]);

  useEffect(() => {
    setProducts(storeProducts);
  }, [storeProducts]);

  const homeButtons = !isAuthenticated ? (
    <>
      <Link to="/register" className="btn btn-lg btn-info mr-2">
        Sign Up
      </Link>
      <Link to="/login" className="btn btn-lg btn-warning">
        Login
      </Link>
    </>
  ) : (
    <></>
  );

  console.log(BASE_URL_API);
  

  return (
    <div className="landing">
      <div className="dark-overlay landing-inner">
        <div className="container">
          <div className="row text-light">
            <div className="col-md-12 text-center">
              <h1 className="display-3 mb-4">BET Shop</h1>
              <p className="lead">
                New Experience, Easy To Shop. Make an Order with us today.
                <Link to="/shopping" className="btn btn-lg btn-danger">
                  Go shopping
                </Link>
              </p>
              <hr />
              {homeButtons}
            </div>
          </div>
          <hr />
          <div className="row">
            {/* <div className="col-md-10"> */}
            {products.map((prod: any) => (
              <div key={prod.id} className="col-md-3 m-4">
                <Card product={prod} />
              </div>
            ))}
            {/* </div> */}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Landing;
