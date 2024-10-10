import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Field, Form, Formik, FormikHelpers } from "formik";
import * as yup from "yup";

interface CategoryValues {
    name: string
}

export default function CategoryCreator() {
    const initialValues: CategoryValues = { name: '' };
    const schema: yup.ObjectSchema<CategoryValues> = yup.object({
        name: yup.string()
            .required()
    });

    const queryClient = useQueryClient();

    const mutation = useMutation({
        mutationFn: async (values: CategoryValues) => await fetch('https://localhost:5001/categories', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ name: values.name })
        }),
        onSuccess: () => queryClient.invalidateQueries(['categories', 'tree'])
    });

    async function submit(values: CategoryValues, { resetForm }: FormikHelpers<CategoryValues>) {
        await mutation.mutateAsync(values);
        resetForm();
    }

    return (
        <div className="ml-2 mb-2">
            <Formik
                initialValues={initialValues}
                validationSchema={schema}
                onSubmit={submit}>
                    <Form>
                        <Field
                            name="name"
                            type="text"
                            placeholder="Create new category"
                            className="px-2 bg-gray-100 focus:bg-gray-200 border border-gray-300" />
                    </Form>
            </Formik>
        </div>
    );
}
