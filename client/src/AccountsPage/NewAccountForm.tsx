import * as yup from 'yup';
import {ErrorMessage, Field, Form, Formik, FormikHelpers} from "formik";
import useCreateAccount from "./CreateAccountCommand.ts";

export interface NewAccountFormValues {
    name: string
}

export interface NewAccountFormProps {
    initialValues?: NewAccountFormValues
}

export default function NewAccountForm({initialValues}: NewAccountFormProps) {
    const values = initialValues ?? {name: ''};

    const schema: yup.ObjectSchema<NewAccountFormValues> = yup.object({
        name: yup.string()
            .required('Name is required')
            .max(100, 'Name must be 100 characters or less')
    });

    const {mutateAsync: create} = useCreateAccount();

    async function submit(values: NewAccountFormValues, formik: FormikHelpers<NewAccountFormValues>) {
        formik.setSubmitting(true);
        await create(values);
        formik.resetForm();
    }

    return (
        <Formik initialValues={values} validationSchema={schema} onSubmit={submit}>
            {props =>
                <Form>
                    <div className="grid grid-rows-[1fr,_min-content] grid-cols-[1fr,_min-content] gap-x-2 w-1/4">
                        <Field name="name"
                               disabled={props.isSubmitting}
                               className={`border ${props.touched.name && props.errors.name ? 'border-red-500' : 'border-gray-300'} disabled:bg-gray-100 disabled:text-gray-300 rounded px-2`}
                               placeholder="Account name"/>
                        <button type="submit" title="New Account"><i className="fa fa-plus"></i></button>
                        {props.touched.name && props.errors.name &&
                            <div className="text-sm text-red-500">
                                <ErrorMessage name="name"/>
                            </div>
                        }
                    </div>
                </Form>
            }
        </Formik>
    )
}