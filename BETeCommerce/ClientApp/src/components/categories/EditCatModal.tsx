import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { updateCategory } from "../../redux/actions/categoryActions";
import isEmpty from "../../validations/is-empty";
import Spinner from "../common/Spinner";
import TextFieldGroup from "../common/TextFieldGroup";
import { Alert } from "../errors/Alert";

interface IModalProps {
  id: string;
  modalId: string;
  category: string;
}

const EditCatModal: React.FC<IModalProps> = ({ id, modalId, category }) => {
  const dispatch = useDispatch();
  const errorState: any = useSelector((state: any) => state.errorReducer);
  const loadingState: any = useSelector(
    (state: any) => state.categoryReducer.loading
  );
  const [categoryName, setCategoryName] = useState(category);
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
      updateCategory({
        id,
        updateDetails: {
          name: categoryName,
        },
      })
    );

    setCategoryName("");
  };

  return (
    <div
      className='modal fade'
      id={modalId}
      tabIndex={-1}
      role='dialog'
      aria-labelledby='exampleModalLabel'
      aria-hidden='true'>
      <div className='modal-dialog' role='document'>
        <form onSubmit={onEditClick}>
          {!isEmpty(errors) && <Alert errors={errors} alertName='danger' />}
          <div className='modal-content'>
            <div className='modal-header'>
              <h5 className='modal-title'>Edit Category</h5>
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
                placeholder='Category Name'
                name='name'
                onChange={(e: any) => setCategoryName(e.target.value)}
                value={categoryName}
                info={""}
                disabled={false}
              />
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
            {loading && isEmpty(errors) && <Spinner />}
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditCatModal;
