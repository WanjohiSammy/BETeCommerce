import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  deleteCategory,
  getCategories,
} from "../../redux/actions/categoryActions";
import AddCatModal from "./AddCatModal";
import EditCatModal from "./EditCatModal";

const ManageCategories = () => {
  const dispatch = useDispatch();
  const storeCategories = useSelector(
    (state: any) => state.categoryReducer.categories
  );

  const [categories, setCategories] = useState(storeCategories);

  useEffect(() => {
    dispatch(getCategories(1, 10));
  }, [dispatch]);

  useEffect(() => {
    setCategories(storeCategories);
  }, [storeCategories]);

  const onDeleteClick = (catId: string) => {
    dispatch(
      deleteCategory({
        id: catId,
      })
    );
  };

  return (
    <div className='container'>
      <button
        className='btn btn-primary btn-lg mt-3 mb-3'
        data-toggle='modal'
        data-target='#addCategoryId'>
        Add Category
      </button>
      <AddCatModal id='addCategoryId' />

      <h4 className='mb-4'>Product Categories</h4>
      <table className='table table-bordered table-hover table-responsive-sm'>
        <thead>
          <tr>
            <th scope='col'>#</th>
            <th scope='col'>Category Name</th>
            <th scope='col'>Actions</th>
          </tr>
        </thead>
        <tbody>
          {categories &&
            categories.map((cat: any, index: number) => (
              <tr key={cat.id}>
                <th scope='row'>{index + 1}</th>
                <td>{cat.name}</td>
                <td colSpan={2}>
                  <button
                    className='btn btn-info btn-sm mr-2'
                    data-toggle='modal'
                    data-target={`#edit_${cat.id}`}>
                    Edit
                  </button>
                  <button
                    className='btn btn-danger btn-sm'
                    onClick={() => onDeleteClick(cat.id)}>
                    Delete
                  </button>
                  <EditCatModal
                    id={cat.id}
                    modalId={`edit_${cat.id}`}
                    category={cat.name}
                  />
                </td>
              </tr>
            ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManageCategories;
