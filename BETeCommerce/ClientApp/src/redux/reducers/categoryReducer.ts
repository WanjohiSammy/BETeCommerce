import {
  GET_CATEGORY,
  ADD_CATEGORY,
  UPDATE_CATEGORY,
  DELETE_CATEGORY,
  CATEGORY_LOADING,
  GET_CATEGORIES,
} from "./../store/types";

const initialState = {
  categories: [],
  category: {},
  count: 0,
  loading: false,
};

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = initialState, action: any) {
  switch (action.type) {
    case CATEGORY_LOADING:
      return {
        ...state,
        loading: true,
      };

    case GET_CATEGORIES:
      return {
        ...state,
        categories: action.payload.categories,
        count: action.payload.count,
        loading: false,
      };

    case GET_CATEGORY:
      return {
        ...state,
        category: action.payload.category,
        loading: false,
      };

    case ADD_CATEGORY:
      return {
        ...state,
        category: action.payload.category,
        categories: [action.payload.category, ...state.categories],
        loading: false,
      };

    case UPDATE_CATEGORY:
      return {
        ...state,
        category: action.payload.category,
        categories: state.categories.map((cat: any) =>
          cat.id === action.payload.category.id ? action.payload.category : cat
        ),
        loading: false,
      };

    case DELETE_CATEGORY:
      return {
        ...state,
        categories: state.categories.filter(
          (cat: any) => cat.id !== action.payload.category.id
        ),
        loading: false,
      };

    default:
      return state;
  }
}
