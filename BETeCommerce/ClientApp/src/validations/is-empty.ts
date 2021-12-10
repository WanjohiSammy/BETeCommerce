const isEmpty = (value: any) =>
  value === undefined ||
  value === null ||
  value === "" ||
  value === '""' ||
  (typeof value === "object" && Object.keys(value).length === 0) ||
  (typeof value === "string" && value && value.trim().length === 0);

export default isEmpty;
