import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router";
import TextFieldGroup from "../../components/common/TextFieldGroup";
import { loginUser } from "../../redux/actions/authActions";
import isEmpty from "../../validations/is-empty";
import Spinner from "../common/Spinner";
import { Alert } from "../errors/Alert";

const Login = () => {
  const navigate = useNavigate();
  const isAuthenticated: boolean = useSelector(
    (state: any) => state.authReducer.isAuthenticated
  );

  const loadingState: boolean = useSelector(
    (state: any) => state.authReducer.loading
  );

  if (isAuthenticated) {
    navigate("/");
  }

  const errorState: any = useSelector((state: any) => state.errorReducer);
  const userState: any = useSelector((state: any) => state.authReducer.user);

  const dispatch = useDispatch();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(loadingState);
  const [errors, setErrors] = useState(errorState);
  const [user, setUser] = useState(userState);

  useEffect(() => {
    setErrors(errorState);
  }, [errorState]);

  useEffect(() => {
    setUser(userState);
  }, [userState]);

  useEffect(() => {
    setLoading(loadingState);
  }, [loadingState]);

  const onSubmit = (e: any) => {
    e.preventDefault();
    setLoading(true);
    dispatch(
      loginUser({
        email,
        password,
      })
    );
  };

  return (
    <div className='login'>
      <div className='container'>
        <div className='row'>
          <div className='col-md-8 m-auto'>
            <h1 className='display-4 text-center'>Log In</h1>
            <p className='lead text-center'>Sign in to BET Shop</p>
            {!isEmpty(errors) && <Alert errors={errors} alertName='danger' />}
            <form onSubmit={onSubmit}>
              <TextFieldGroup
                type='email'
                error={""}
                placeholder='Email Address'
                name='email'
                onChange={(e: any) => setEmail(e.target.value)}
                value={email}
                info={""}
                disabled={false}
                required={true}
              />
              <TextFieldGroup
                key='password'
                type='password'
                error={""}
                placeholder='Password'
                name='password'
                onChange={(e: any) => setPassword(e.target.value)}
                value={password}
                info={""}
                disabled={false}
                required={true}
              />
              <input
                type='submit'
                className='btn btn-info btn-block mt-4'
                disabled={loading}
              />
              {loading && <Spinner />}
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
