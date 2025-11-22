import * as yup from "yup";
import { ErrorMessage, Field, Form, Formik, FormikHelpers } from "formik";

export interface AccountFormValues {
  name: string;
}

export interface AccountFormProps {
  initialValues?: AccountFormValues;
  autoFocus: boolean;
  editing: boolean;
  onSubmit: (values: AccountFormValues) => any | Promise<any>;
}

export default function AccountForm({
  initialValues,
  autoFocus,
  editing,
  onSubmit,
}: AccountFormProps) {
  const values = initialValues ?? { name: "" };

  const schema: yup.ObjectSchema<AccountFormValues> = yup.object({
    name: yup
      .string()
      .required("Name is required")
      .max(100, "Name must be 100 characters or less"),
  });

  async function submit(
    values: AccountFormValues,
    formik: FormikHelpers<AccountFormValues>,
  ) {
    formik.setSubmitting(true);
    await onSubmit(values);
    formik.resetForm();
  }

  return (
    <Formik initialValues={values} validationSchema={schema} onSubmit={submit}>
      {(props) => (
        <Form>
          <div className="grid grid-rows-[1fr,_min-content] grid-cols-[1fr,_min-content] gap-x-2 w-1/4">
            <Field
              name="name"
              autoFocus={autoFocus}
              disabled={props.isSubmitting}
              className={`border ${props.touched.name && props.errors.name ? "border-red-500" : "border-gray-300"} disabled:bg-gray-100 disabled:text-gray-300 rounded px-2`}
              placeholder="Account name"
            />
            <button type="submit" title="New Account">
              {!editing && <i className="fa fa-plus"></i>}
              {editing && <i className="fa fa-check"></i>}
            </button>
            {props.touched.name && props.errors.name && (
              <div className="text-sm text-red-500">
                <ErrorMessage name="name" />
              </div>
            )}
          </div>
        </Form>
      )}
    </Formik>
  );
}
