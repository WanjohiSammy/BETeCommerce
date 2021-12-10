import "./App.css";
import Navbar from "./hoc/Navbar";
import { Route, Routes } from "react-router-dom";
import Landing from "./hoc/Landing";
import Register from "./components/auth/Register";
import Login from "./components/auth/Login";
import Footer from "./hoc/Footer";
import ManageCategories from "./components/categories/ManageCategories";
import ManageProducts from "./components/products/ManageProducts";
import PrivateRoute from "./components/common/PrivateRoute";
import Shopping from "./components/shopping/Shopping";
import Checkout from "./components/shopping/Checkout";

function App() {
  return (
    <div className='App'>
      <Navbar />
      <Routes>
        <Route path='/' element={<Landing />} />
        <Route path='/register' element={<Register />} />
        <Route path='/login' element={<Login />} />
        <Route
          path='/categories'
          element={
            <PrivateRoute>
              <ManageCategories />
            </PrivateRoute>
          }
        />
        <Route
          path='/products'
          element={
            <PrivateRoute>
              <ManageProducts />
            </PrivateRoute>
          }
        />
        <Route
          path='/checkout'
          element={
            <PrivateRoute>
              <Checkout />
            </PrivateRoute>
          }
        />
        <Route path='/shopping' element={<Shopping />} />
      </Routes>
      <Footer />
    </div>
  );
}

export default App;
