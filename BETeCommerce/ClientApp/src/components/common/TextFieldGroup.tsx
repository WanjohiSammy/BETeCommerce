import React from "react";
import classnames from "classnames";

interface ITextFieldGroupProps {
  name: string;
  placeholder: any;
  value?: any;
  id?: any;
  error: string;
  info: string;
  type: string;
  onChange: any;
  disabled: boolean;
  required?: boolean;
}

const TextFieldGroup: React.FC<ITextFieldGroupProps> = ({
  name,
  placeholder,
  value,
  error,
  info,
  type,
  id,
  onChange,
  required,
  disabled,
}) => {
  return (
    <div className='form-group'>
      <label >{placeholder}</label>
      <input
        key={name}
        type={type}
        className={classnames("form-control form-control-lg", {
          "is-invalid": error,
        })}
        placeholder={placeholder}
        name={name}
        id={id ? id : name}
        onChange={onChange}
        value={value}
        disabled={disabled}
        required={required}
      />
      {info && <small className='form-text text-muted'>{info}</small>}
      {error && <div className='invalid-feedback'>{error}</div>}
    </div>
  );
};

export default TextFieldGroup;
