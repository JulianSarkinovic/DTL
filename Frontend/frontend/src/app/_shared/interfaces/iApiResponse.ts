export interface IApiResponseEmpty {
  success: boolean;
  errorMessage?: string;
  route?: string;
}

export interface IApiResponse<TResult> extends IApiResponseEmpty {
  resultData?: TResult;
}
