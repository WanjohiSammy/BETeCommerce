import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import isEmpty from "../../validations/is-empty";
import Spinner from "../common/Spinner";
import TextFieldGroup from "../common/TextFieldGroup";
import { Alert } from "../errors/Alert";
import { updateProductDetails } from "./../../redux/actions/productActions";

interface IModalProps {
  id: string;
  modalId: string;
  product: any;
}

const EditProductModal: React.FC<IModalProps> = ({ id, modalId, product }) => {
  const dispatch = useDispatch();
  const errorState: any = useSelector((state: any) => state.errorReducer);
  const loadingState: any = useSelector(
    (state: any) => state.productReducer.loading
  );

  const [productName, setProductName] = useState(product.name);
  const [quantity, setQuantity] = useState(product.quantity);
  const [price, setPrice] = useState(product.price);
  const [loading, setLoading] = useState(loadingState);
  const [errors, setErrors] = useState(errorState);

  useEffect(() => {
    setLoading(loadingState);
  }, [loadingState]);

  useEffect(() => {
    setErrors(errorState);
  }, [errorState]);

  const onEditClick = (e: any) => {
    e.preventDefault();

    dispatch(
      updateProductDetails({
        name: product.name,
        id: product.id,
        quantity,
        price,
        categoryId: product.categoryId,
      })
    );
  };

  return (
    <div
      className="modal fade"
      id={modalId}
      tabIndex={-1}
      role="dialog"
      aria-labelledby="exampleModalLabel"
      aria-hidden="true"
    >
      <div className="modal-dialog" role="document">
        <form onSubmit={onEditClick}>
          {!isEmpty(errors) && <Alert errors={errors} alertName="danger" />}
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">Edit Product</h5>
              <button
                type="button"
                className="close"
                data-dismiss="modal"
                aria-label="Close"
              >
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">
              <TextFieldGroup
                type="text"
                error={""}
                placeholder="Product Name"
                name="name"
                onChange={(e: any) => setProductName(e.target.value)}
                value={productName}
                info={""}
                required={true}
                disabled={false}
              />

              <TextFieldGroup
                type="number"
                error={""}
                placeholder="Price per item"
                name="price"
                onChange={(e: any) => setPrice(e.target.value)}
                value={price}
                info={""}
                disabled={false}
              />

              <TextFieldGroup
                type="number"
                error={""}
                placeholder="Quantity"
                name="quantity"
                onChange={(e: any) => setQuantity(e.target.value)}
                value={quantity}
                info={""}
                required={true}
                disabled={false}
              />
            </div>
            <div className="modal-footer">
              <input
                type="submit"
                className="btn btn-primary"
                disabled={loading && !isEmpty(errors)}
              />
              <button
                type="button"
                className="btn btn-secondary"
                data-dismiss="modal"
              >
                Close
              </button>
              {loading && isEmpty(errors) && <Spinner />}
            </div>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditProductModal;
