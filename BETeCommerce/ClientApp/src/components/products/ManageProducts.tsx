import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { deleteProduct, getProducts } from "../../redux/actions/productActions";
import AddProductModal from "./AddProductModal";
import EditProductModal from "./EditProductModal";

const ManageProducts = () => {
  const dispatch = useDispatch();
  const storeProducts = useSelector(
    (state: any) => state.productReducer.products
  );
  const storeCategories = useSelector(
    (state: any) => state.categoryReducer.categories
  );

  useEffect(() => {
    dispatch(getProducts(1, 10));
  }, [dispatch]);

  const [products, setProducts] = useState(storeProducts);
  const [categories, setCategories] = useState(storeCategories);
  
  useEffect(() => {
    setProducts(storeProducts);
  }, [storeProducts]);

  useEffect(() => {
    setCategories(storeCategories);
  }, [storeCategories]);

  const onDeleteClick = (catId: string) => {
    dispatch(
      deleteProduct({
        id: catId,
      })
    );
  };

  return (
    <div className='container'>
      <button
        className='btn btn-primary btn-lg mt-3 mb-3'
        data-toggle='modal'
        data-target='#addProductId'>
        Add Product
      </button>
      <AddProductModal modalId='addProductId' categories={categories} />

      <h4 className='mb-4'>Product Categories</h4>
      <table className='table table-bordered table-hover table-responsive-sm'>
        <thead>
          <tr>
            <th scope='col'>#</th>
            <th scope='col'>Product Name</th>
            <th scope='col'>Category Name</th>
            <th scope='col'>Quantity</th>
            <th scope='col'>Price per item(Ksh)</th>
            <th scope='col'>Seller Email</th>
            <th scope='col'>Actions</th>
          </tr>
        </thead>
        <tbody>
          {products &&
            products.map((prod: any, index: number) => (
              <tr key={prod.id}>
                <th scope='row'>{index + 1}</th>
                <td>{prod.name}</td>
                <td>{prod.categoryName}</td>
                <td>{prod.quantity}</td>
                <td>{prod.price}</td>
                <td>{prod.sellerEmailAddress}</td>
                <td colSpan={2}>
                  <button
                    className='btn btn-info btn-sm mr-2'
                    data-toggle='modal'
                    data-target={`#edit_${prod.id}`}>
                    Edit
                  </button>
                  <button
                    className='btn btn-danger btn-sm'
                    onClick={() => onDeleteClick(prod.id)}>
                    Delete
                  </button>
                  <EditProductModal
                    id={prod.id}
                    modalId={`edit_${prod.id}`}
                    product={prod}
                  />
                </td>
              </tr>
            ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManageProducts;
