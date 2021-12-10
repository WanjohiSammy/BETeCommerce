import React from "react";
import classnames from "classnames";

interface ISelectListGroup {
  name: string;
  value: any;
  error: string;
  info: string;
  options: any;
  onChange: any;
  title: string;
  required? : boolean
}

const SelectListGroup: React.FC<ISelectListGroup> = ({
  name,
  value,
  required,
  error,
  title,
  info,
  onChange,
  options,
}) => {
  const defaultOptions = <option>{"Select " + title}</option>;

  const selectOptions = options.map((option: any) => (
    <option key={option.label} value={option.value}>
      {option.label}
    </option>
  ));
  return (
    <div className="form-group">
      <label>{title}</label>
      <select
        className={classnames("form-control form-control-lg", {
          "is-invalid": error,
        })}
        name={name}
        onChange={onChange}
        value={value}
        required={required}
      >
        {defaultOptions}
        {selectOptions}
      </select>
      {info && <small className="form-text text-muted">{info}</small>}
      {error && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};

export default SelectListGroup;
