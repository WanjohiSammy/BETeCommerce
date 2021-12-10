import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { addProduct } from "../../redux/actions/productActions";
import isEmpty from "../../validations/is-empty";
import SelectListGroup from "../common/SelectListGroup";
import Spinner from "../common/Spinner";
import TextFieldGroup from "../common/TextFieldGroup";
import { Alert } from "../errors/Alert";
import { getAllCategories } from "./../../redux/actions/categoryActions";

interface IModalProps {
  modalId: string;
  categories: Array<any>;
}

const AddProductModal: React.FC<IModalProps> = ({ modalId, categories }) => {
  const dispatch = useDispatch();
  const errorState: any = useSelector((state: any) => state.errorReducer);
  const loadingState: any = useSelector(
    (state: any) => state.productReducer.loading
  );

  useEffect(() => {
    dispatch(getAllCategories());
  }, [dispatch]);

  const categoryOptions = [];
  if (categories) {
    for (let item of categories) {
      categoryOptions.push({ label: item.name, value: item.id });
    }
  }

  const [loading, setLoading] = useState(loadingState);
  const [errors, setErrors] = useState(errorState);
  const [productName, setProductName] = useState("");
  const [quantity, setQuantity] = useState(1);
  const [price, setPrice] = useState(1);
  const [categoryId, setCategoryId] = useState("");
  const [selectedFile, setSelectedFile] = useState() as any;
  const [imagePreviewUrl, setimagePreviewUrl] = useState() as any;

  // create a preview as a side effect, whenever selected file is changed
  useEffect(() => {
    if (!selectedFile) {
      setimagePreviewUrl(undefined);
      return;
    }

    const objectUrl = URL.createObjectURL(selectedFile);
    setimagePreviewUrl(objectUrl);

    // free memory when ever this component is unmounted
    return () => URL.revokeObjectURL(objectUrl);
  }, [selectedFile]);

  useEffect(() => {
    setLoading(loadingState);
  }, [loadingState]);

  useEffect(() => {
    setErrors(errorState);
  }, [errorState]);

  const handleImageChange = (e: any) => {
    if (!e.target.files || e.target.files.length === 0) {
      setSelectedFile(undefined);
      return;
    }

    // I've kept this example simple by using the first image instead of multiple
    setSelectedFile(e.target.files[0]);
  };

  const onSubmit = (e: any) => {
    e.preventDefault();
    const formData = new FormData();
    formData.append("name", productName);
    formData.append("quantity", quantity.toString());
    formData.append("price", price.toString());
    formData.append("productImage", selectedFile);
    formData.append("categoryId", categoryId);

    dispatch(addProduct(formData));

    setProductName("");
    setQuantity(1);
    setPrice(1);
    setCategoryId("");
    setSelectedFile() as any;
    setimagePreviewUrl() as any;
  };

  let imagePreview;
  if (imagePreviewUrl) {
    imagePreview = (
      <img src={imagePreviewUrl} alt='ProductImage' className={"img-preview"} />
    );
  } else {
    imagePreview = <div className='previewText'>Please select an image.</div>;
  }

  return (
    <div
      className='modal fade'
      id={modalId}
      tabIndex={-1}
      role='dialog'
      aria-labelledby='exampleModalLabel'
      aria-hidden='true'>
      <div className='modal-dialog' role='document'>
        <form onSubmit={onSubmit}>
          {!isEmpty(errors) && <Alert errors={errors} alertName='danger' />}
          <div className='modal-content'>
            <div className='modal-header'>
              <h5 className='modal-title'>Add Product Category</h5>
              <button
                type='button'
                className='close'
                data-dismiss='modal'
                aria-label='Close'>
                <span aria-hidden='true'>&times;</span>
              </button>
            </div>
            <div className='modal-body'>
              <TextFieldGroup
                type='text'
                error={""}
                placeholder='Product Name'
                name='name'
                onChange={(e: any) => setProductName(e.target.value)}
                value={productName}
                info={""}
                disabled={false}
              />
              <TextFieldGroup
                type='number'
                error={""}
                placeholder='Price per item'
                name='price'
                onChange={(e: any) => setPrice(e.target.value)}
                value={price}
                info={""}
                disabled={false}
              />
              <TextFieldGroup
                type='number'
                error={""}
                placeholder='How many items do you have in Store?'
                name='quantity'
                onChange={(e: any) => setQuantity(e.target.value)}
                value={quantity}
                info={""}
                disabled={false}
              />
              <SelectListGroup
                error={""}
                name='categoryId'
                onChange={(e: any) => setCategoryId(e.target.value)}
                value={categoryId}
                info={""}
                options={categoryOptions}
                title="Category"
                required={true}
              />
              <TextFieldGroup
                type='file'
                error={""}
                placeholder='Upload product image'
                name='productImage'
                onChange={(e: any) => handleImageChange(e)}
                info={""}
                disabled={false}
              />
              <div className='img-preview'>{imagePreview}</div>
            </div>
            <div className='modal-footer'>
              <input
                type='submit'
                className='btn btn-primary'
                disabled={loading && !isEmpty(errors)}
              />
              <button
                type='button'
                className='btn btn-secondary'
                data-dismiss='modal'>
                Close
              </button>
            </div>
            {loading && !isEmpty(errors) && <Spinner />}
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddProductModal;
