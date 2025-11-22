import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Field, Form, Formik, FormikHelpers } from "formik";
import * as yup from "yup";

export interface TransactionFormValues {
  timestamp: Date;
  amount: number;
  description: string;
  account: string;
  category?: string | null;
}

export interface TransactionRawFormValues {
  timestamp: string;
  amount: string;
  description: string;
  account: string;
  category: string;
}

export default function TransactionForm() {
  const queryClient = useQueryClient();

  const schema: yup.ObjectSchema<TransactionFormValues> = yup.object({
    timestamp: yup
      .date()
      .max(new Date(), "Cannot be in the future")
      .required("Required"),
    amount: yup
      .number()
      .required("Required")
      .test("nonzero", "Must be non-zero", (x) => x !== 0),
    description: yup.string().required("Required"),
    account: yup.string().required("Required"),
    category: yup.string(),
  });

  const initialValues: TransactionRawFormValues = {
    timestamp: "",
    amount: "",
    description: "",
    account: "",
    category: "",
  };

  const { mutateAsync } = useMutation({
    mutationFn: async (values: TransactionFormValues) =>
      fetch("https://localhost:5001/transactions", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(values),
      }),
  });

  async function submit(
    rawValues: TransactionRawFormValues,
    { setSubmitting, resetForm }: FormikHelpers<TransactionRawFormValues>,
  ) {
    // I'm not sure if I'm not using Formik right or if it's just bad, but we're in the "make it work" stage. I'll make it work better later(tm)
    setSubmitting(true);
    const values = {
      timestamp: new Date(rawValues.timestamp),
      amount: Number(rawValues.amount),
      description: rawValues.description,
      account: rawValues.account,
      category: rawValues.category.trim() === "" ? null : rawValues.category,
    };

    const response = await mutateAsync(values);
    if (response.status === 204) {
      resetForm();
    }
    setSubmitting(false);
    queryClient.invalidateQueries(["transactions"]);
  }

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={schema}
      onSubmit={submit}
    >
      {(props) => (
        <Form>
          <div className="mb-1 grid grid-rows-[min-content,_min-content,_min-content] grid-cols-[240px_1fr_6fr_8rem_2fr,_min-content] gap-1">
            <label htmlFor="timestamp">Timestamp:</label>
            <label htmlFor="account">Account:</label>
            <label htmlFor="description">Description:</label>
            <label htmlFor="amount">Amount:</label>
            <label htmlFor="category">Category:</label>
            <div></div>
            <Field
              id="timestamp"
              name="timestamp"
              type="datetime-local"
              disabled={props.isSubmitting}
              className={`border rounded px-1 disabled:bg-gray-150 disabled:text-gray-400 ${props.touched.timestamp && props.errors.timestamp ? "border-red-500" : ""}`}
            />
            <Field
              id="account"
              name="account"
              disabled={props.isSubmitting}
              className={`border rounded px-1 disabled:bg-gray-150 disabled:text-gray-400 ${props.touched.account && props.errors.account ? "border-red-500" : ""}`}
            />
            <Field
              id="description"
              name="description"
              disabled={props.isSubmitting}
              className={`border rounded px-1 disabled:bg-gray-150 disabled:text-gray-400 ${props.touched.description && props.errors.description ? "border-red-500" : ""}`}
            />
            <Field
              id="amount"
              name="amount"
              type="number"
              disabled={props.isSubmitting}
              className={`border rounded px-1 disabled:bg-gray-150 disabled:text-gray-400 ${props.touched.amount && props.errors.amount ? "border-red-500" : ""}`}
            />
            <Field
              id="category"
              name="category"
              disabled={props.isSubmitting}
              className={`border rounded px-1 disabled:bg-gray-150 disabled:text-gray-400 ${props.touched.category && props.errors.category ? "border-red-500" : ""}`}
            />
            <button type="submit" className="text-gray-700 mx-2">
              <i className="fa fa-arrow-right"></i>
            </button>
            <p className="text-sm text-red-500">
              {props.touched.timestamp &&
                props.errors.timestamp &&
                props.errors.timestamp}
            </p>
            <p className="text-sm text-red-500">
              {props.touched.amount &&
                props.errors.amount &&
                props.errors.amount}
            </p>
            <p className="text-sm text-red-500">
              {props.touched.description &&
                props.errors.description &&
                props.errors.description}
            </p>
            <p className="text-sm text-red-500">
              {props.touched.account &&
                props.errors.account &&
                props.errors.account}
            </p>
            <p className="text-sm text-red-500">
              {props.touched.category &&
                props.errors.category &&
                props.errors.category}
            </p>
          </div>
        </Form>
      )}
    </Formik>
  );
}
