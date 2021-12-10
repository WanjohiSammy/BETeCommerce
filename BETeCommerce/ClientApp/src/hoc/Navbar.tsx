/* eslint-disable jsx-a11y/anchor-is-valid */
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";
import { useAuth } from "../components/customHooks/useAuth";
import { logoutUser } from "../redux/actions/authActions";
import { getCartItems } from "../redux/actions/cartsActions";

const Navbar = () => {
  const dispatch = useDispatch();
  const auth = useAuth();

  let isAuthenticated: boolean = useSelector(
    (state: any) => state.authReducer.isAuthenticated
  );

  if(!isAuthenticated) {
    isAuthenticated = auth.isAuthenticated;
  }

  const storeCartItemsCount = useSelector(
    (state: any) => state.cartsReducer.count
  ); 
  
  const [cartCount, setCartCount] = useState(storeCartItemsCount);
  const [useIsAuthenticated, setUserIsAuthenticated] = useState(isAuthenticated);

  useEffect(() => {
    setUserIsAuthenticated(isAuthenticated)
  }, [isAuthenticated])

  useEffect(() => {
    dispatch(getCartItems())
  },[dispatch]);

  useEffect(() => {
    setCartCount(storeCartItemsCount)
  }, [storeCartItemsCount]);

  const { user } = auth;

  const { given_name } = user;

  const onLogoutClick = () => {
    dispatch(logoutUser());
  };

  const cartLink = (
    <Link className='nav-link' to='/checkout'>
          <button className='btn btn-primary'>
            Cart <span className='badge badge-light'>{cartCount}</span>
          </button>
        </Link>
  );

  const authLinks = (
    <ul className='navbar-nav ml-auto'>
      <li className='nav-item'>
        {cartLink}
      </li>
      <li className='nav-item'>
        <a href='' className='nav-link' onClick={onLogoutClick}>
          <img
            className='rounded-circle'
            src=''
            alt={""}
            style={{ width: "25px", marginRight: "5px" }}
          />
          <button className='btn btn-outline-warning'>
            {given_name} Logout
          </button>
        </a>
      </li>
    </ul>
  );

  const guestLinks = (
    <ul className='navbar-nav ml-auto'>
      <li className='nav-item'>
        {cartLink}
      </li>
      <li className='nav-item'>
        <Link className='nav-link' to='/register'>
          <button className='btn btn-info mr-2'>Sign Up</button>
        </Link>
      </li>
      <li className='nav-item'>
        <Link className='nav-link' to='/login'>
          <button className='btn btn-secondary'>Login</button>
        </Link>
      </li>
    </ul>
  );

  return (
    <nav className='navbar navbar-expand-sm navbar-dark bg-dark mb-4'>
      <div className='container'>
        <Link className='navbar-brand' to='/'>
          BET Shop
        </Link>
        <button
          className='navbar-toggler'
          type='button'
          data-toggle='collapse'
          data-target='#mobile-nav'>
          <span className='navbar-toggler-icon' />
        </button>

        <div className='collapse navbar-collapse' id='mobile-nav'>
          <ul className='navbar-nav mr-auto'>
            <li className='nav-item'>
              <Link className='nav-link' to='/categories'>
                Categories
              </Link>
            </li>
            <li className='nav-item'>
              <Link className='nav-link' to='/products'>
                Add Product
              </Link>
            </li>
          </ul>
          {useIsAuthenticated ? authLinks : guestLinks}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
