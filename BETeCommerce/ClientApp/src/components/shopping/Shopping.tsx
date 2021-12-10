import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import Card from "./Card";
import {
  getAllCategories,
  getProductsByCategories,
} from "./../../redux/actions/categoryActions";
import isEmpty from "./../../validations/is-empty";

const Shopping = () => {
  const dispatch = useDispatch();

  const storeCategories = useSelector(
    (state: any) => state.categoryReducer.categories
  );

  const storeProducts = useSelector(
    (state: any) => state.productReducer.products
  );

  const [btnColor, setBtnColor] = useState("");
  const [products, setProducts] = useState(storeProducts);

  useEffect(() => {
    setProducts(storeProducts);
  }, [storeProducts]);

  useEffect(() => {
    dispatch(getAllCategories());
  }, [dispatch]);

  const onClickCategory = (e: any) => {
    setBtnColor(e.target.id);
    dispatch(getProductsByCategories(e.target.id, 1, 10));
  };

  return (
    <div className='container'>
      <h3 className='lead'>Filter by Category</h3>
      {storeCategories.map((cat: any) => (
        <button
          key={cat.id}
          id={cat.id}
          name={cat.name}
          className={`btn ${
            !isEmpty(btnColor) && btnColor === cat.id
              ? "btn-info"
              : "btn-outline-info"
          } m-2`}
          onClick={onClickCategory}>
          {cat.name}
        </button>
      ))}
      <hr />
      <h2>Add to Cart</h2>
      <div className='row'>
        {products.map((prod: any) => (
          <div key={prod.id} className='col-md-3 m-4'>
            <Card product={prod} />
          </div>
        ))}
      </div>
    </div>
  );
};

export default Shopping;
