export type ErrorOr<Type> =  {
  isError: boolean;
  errors: Error[];
  errorsOrEmptyList: Error[];
  value: Type;
  firstError: Error;
}

interface Error {
  code: string;
  description: string;
  type: number;
  numericType: number;
}
