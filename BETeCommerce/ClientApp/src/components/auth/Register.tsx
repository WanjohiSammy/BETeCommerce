import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import TextFieldGroup from "../../components/common/TextFieldGroup";
import { registerUser } from "../../redux/actions/authActions";
import isEmpty from "../../validations/is-empty";
import Spinner from "../common/Spinner";
import { Alert } from "../errors/Alert";

const Register = () => {
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
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [user, setUser] = useState(userState);
  const [errors, setErrors] = useState(errorState);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(loadingState);
  }, [loadingState]);

  useEffect(() => {
    setErrors(errorState);
  }, [errorState]);

  useEffect(() => {
    setUser(userState);
  }, [userState]);

  const onSubmit = (e: any) => {
    e.preventDefault();
    setLoading(true);

    dispatch(
      registerUser({
        fullName,
        email,
        password,
        confirmPassword,
      })
    );
  };

  return (
    <div className='register'>
      <div className='container'>
        <div className='row'>
          <div className='col-md-8 m-auto'>
            <h1 className='display-4 text-center'>Sign Up</h1>
            <p className='lead text-center'>Create BET Shop account</p>
            {!isEmpty(errors) && <Alert errors={errors} alertName='danger' />}
            <form onSubmit={onSubmit}>
              <TextFieldGroup
                type='text'
                error={""}
                placeholder='FullName'
                name='fullName'
                onChange={(e: any) => setFullName(e.target.value)}
                value={fullName}
                info={""}
                disabled={false}
                required={true}
              />

              <TextFieldGroup
                type='email'
                error={""}
                placeholder='Email Address'
                name='email'
                onChange={(e: any) => setEmail(e.target.value)}
                value={email}
                info=''
                disabled={false}
                required={true}
              />

              <TextFieldGroup
                type='password'
                error={""}
                placeholder='Password'
                name='password'
                onChange={(e: any) => setPassword(e.target.value)}
                value={password}
                disabled={false}
                info={""}
                required={true}
              />

              <TextFieldGroup
                type='password'
                error={""}
                placeholder='Confirm Password'
                name='confirmPassword'
                onChange={(e: any) => setConfirmPassword(e.target.value)}
                value={confirmPassword}
                disabled={false}
                info={""}
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

export default Register;
