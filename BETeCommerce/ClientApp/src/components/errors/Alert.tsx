import React from "react";

interface IAlert {
  errors: any;
  alertName: string;
}

export const Alert: React.FC<IAlert> = ({ errors, alertName }) => {
  const alertClassName = `alert alert-${alertName}`;
  return (
    <div className={alertClassName} role='alert'>
      {JSON.stringify(errors)}
    </div>
  );
};
