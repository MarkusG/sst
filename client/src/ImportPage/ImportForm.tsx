import {Field, Form, Formik} from "formik";
import * as yup from 'yup';
import {useQuery} from "@tanstack/react-query";

export interface ImportFormValues {
    accountName: string,
    file?: File
}

export interface ImportFormProps {
    onSubmit: (values: ImportFormValues) => any | Promise<any>;
}

export default function ImportForm({onSubmit}: ImportFormProps) {
    const initialValues = {accountName: ''};

    const schema: yup.ObjectSchema<ImportFormValues> = yup.object({
        accountName: yup.string()
            .required('Account Name is required'),
        file: yup.mixed<File>()
            .required('File is required')
    });

    const {data} = useQuery<string[]>({
        queryKey: ['transaction-accounts'],
        queryFn: async () => await fetch('https://localhost:5001/transaction-accounts').then(r => r.json())
    })

    return (
        <Formik initialValues={initialValues} validationSchema={schema} onSubmit={onSubmit}>
            {props =>
                <Form>
                    <Field name="accountName" list="accountList" placeholder="Account Name"/>
                    <datalist id="accountList">
                        {data && data.map(a => <option key={a}>{a}</option>)}
                    </datalist>
                    <input type="file"
                           onChange={e => props.setFieldValue('file', e.currentTarget.files !== null ? e.currentTarget.files[0] : null)}/>
                    <button>Submit</button>
                </Form>
            }
        </Formik>
    )
}